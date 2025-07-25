﻿/*
	UserInterface.JS
	Contains Functions and Objects for the UI
*/

/* The Window.UI object definition */
window.UI = {

	ShowBlockingLoadingMessage: function (_Message) {
		window.Utilities.LogDebug("Showing Blocking-LoadingBox (with message [" + _Message + "])...");
		Swal.fire({
			title: "Please Wait...",
			html: "<img src='/ClientPages/Resources/Images/Loading.GIF' class='img-fluid' width='20%' /><br/><br/><br/>" + _Message,
			showConfirmButton: false,
			allowOutsideClick: false,
			allowEscapeKey: false
		});
	},

	DismissBlockingLoadingMessage: function (_Message) {
		window.Utilities.LogDebug("...Dismissing Blocking-LoadingBox");
		swal.close();
	}

};

/* A utility function for the ACE in-browser syntax highlighting */
function InitialiseACEEditor(_EditorDivID) {
	
	/* REQUIRES THE ACE SCRIPTS IN AceEditor\ */
	window.DSSourceEditor_Object = ace.edit(_EditorDivID);
	window.DSSourceEditor_Object.setTheme("ace/theme/sqlserver");
	var _TCLMode = ace.require("ace/mode/tcl").Mode;
	window.DSSourceEditor_Object.session.setMode(new _TCLMode());
	window.DSSourceEditor_Object.setShowPrintMargin(false);
	
	window.DSSourceEditor_Object.setOptions({
		fontFamily : "Inconsolata Variable Weight,Consolas,Courier New",
		fontSize : "16pt",
		autoScrollEditorIntoView : true,
		copyWithEmptySelection : true
	});

}

/* Extention Methods for removing HTML Elements from the DOM */
Element.prototype.remove = function() { this.parentElement.removeChild(this); }
NodeList.prototype.remove = HTMLCollection.prototype.remove = function() {
	for(var i = this.length - 1; i >= 0; i--) {
		if(this[i] && this[i].parentElement) {
			this[i].parentElement.removeChild(this[i]);
		}
	}
}


/*
	The rest of this file is...
	Canvas2image - A tool of saving or converting canvas to images
	https://github.com/hongru/canvas2image
*/
const Canvas2Image = (function () {
	// check if support sth.
	const $support = (function () {
		const canvas = document.createElement("canvas"),
			ctx = canvas.getContext("2d");

		return {
			canvas: !!ctx,
			imageData: !!ctx.getImageData,
			dataURL: !!canvas.toDataURL,
			btoa: !!window.btoa,
		};
	})();

	const downloadMime = "image/octet-stream";

	function scaleCanvas(canvas, width, height) {
		const w = canvas.width,
			h = canvas.height;
		if (width === undefined) {
			width = w;
		}
		if (height === undefined) {
			height = h;
		}

		let retCanvas = document.createElement("canvas");
		let retCtx = retCanvas.getContext("2d");
		retCanvas.width = width;
		retCanvas.height = height;
		retCtx.drawImage(canvas, 0, 0, w, h, 0, 0, width, height);
		return retCanvas;
	}

	function getDataURL(canvas, type, width, height) {
		canvas = scaleCanvas(canvas, width, height);
		return canvas.toDataURL(type);
	}

	// save file to local with file name and file type
	function saveFile(strData, fileType, fileName = "name") {
		// document.location.href = strData;
		let saveLink = document.createElement("a");
		// download file name
		saveLink.download = fileName + "." + fileType;
		// download file data
		saveLink.href = strData;
		// start download
		saveLink.click();
	}

	function genImage(strData) {
		let img = document.createElement("img");
		img.src = strData;
		return img;
	}

	function fixType(type) {
		type = type.toLowerCase().replace(/jpg/i, "jpeg");
		const r = type.match(/png|jpeg|bmp|gif/)[0];
		return "image/" + r;
	}

	function encodeData(data) {
		if (!window.btoa) {
			// eslint-disable-next-line no-throw-literal
			throw "btoa undefined";
		}
		let str = "";
		if (typeof data == "string") {
			str = data;
		} else {
			for (let i = 0; i < data.length; i++) {
				str += String.fromCharCode(data[i]);
			}
		}

		return btoa(str);
	}

	function getImageData(canvas) {
		const w = canvas.width,
			h = canvas.height;
		return canvas.getContext("2d").getImageData(0, 0, w, h);
	}

	function makeURI(strData, type) {
		return "data:" + type + ";base64," + strData;
	}

	/**
	 * create bitmap image
	 * 按照规则生成图片响应头和响应体
	 */
	const genBitmapImage = function (oData) {
		//
		// BITMAPFILEHEADER: http://msdn.microsoft.com/en-us/library/windows/desktop/dd183374(v=vs.85).aspx
		// BITMAPINFOHEADER: http://msdn.microsoft.com/en-us/library/dd183376.aspx
		//

		const biWidth = oData.width;
		const biHeight = oData.height;
		const biSizeImage = biWidth * biHeight * 3;
		const bfSize = biSizeImage + 54; // total header size = 54 bytes

		//
		//  typedef struct tagBITMAPFILEHEADER {
		//  	WORD bfType;
		//  	DWORD bfSize;
		//  	WORD bfReserved1;
		//  	WORD bfReserved2;
		//  	DWORD bfOffBits;
		//  } BITMAPFILEHEADER;
		//
		const BITMAPFILEHEADER = [
			// WORD bfType -- The file type signature; must be "BM"
			0x42,
			0x4d,
			// DWORD bfSize -- The size, in bytes, of the bitmap file
			bfSize & 0xff,
			(bfSize >> 8) & 0xff,
			(bfSize >> 16) & 0xff,
			(bfSize >> 24) & 0xff,
			// WORD bfReserved1 -- Reserved; must be zero
			0,
			0,
			// WORD bfReserved2 -- Reserved; must be zero
			0,
			0,
			// DWORD bfOffBits -- The offset, in bytes, from the beginning of the BITMAPFILEHEADER structure to the bitmap bits.
			54,
			0,
			0,
			0,
		];

		//
		//  typedef struct tagBITMAPINFOHEADER {
		//  	DWORD biSize;
		//  	LONG  biWidth;
		//  	LONG  biHeight;
		//  	WORD  biPlanes;
		//  	WORD  biBitCount;
		//  	DWORD biCompression;
		//  	DWORD biSizeImage;
		//  	LONG  biXPelsPerMeter;
		//  	LONG  biYPelsPerMeter;
		//  	DWORD biClrUsed;
		//  	DWORD biClrImportant;
		//  } BITMAPINFOHEADER, *PBITMAPINFOHEADER;
		//
		const BITMAPINFOHEADER = [
			// DWORD biSize -- The number of bytes required by the structure
			40,
			0,
			0,
			0,
			// LONG biWidth -- The width of the bitmap, in pixels
			biWidth & 0xff,
			(biWidth >> 8) & 0xff,
			(biWidth >> 16) & 0xff,
			(biWidth >> 24) & 0xff,
			// LONG biHeight -- The height of the bitmap, in pixels
			biHeight & 0xff,
			(biHeight >> 8) & 0xff,
			(biHeight >> 16) & 0xff,
			(biHeight >> 24) & 0xff,
			// WORD biPlanes -- The number of planes for the target device. This value must be set to 1
			1,
			0,
			// WORD biBitCount -- The number of bits-per-pixel, 24 bits-per-pixel -- the bitmap
			// has a maximum of 2^24 colors (16777216, Truecolor)
			24,
			0,
			// DWORD biCompression -- The type of compression, BI_RGB (code 0) -- uncompressed
			0,
			0,
			0,
			0,
			// DWORD biSizeImage -- The size, in bytes, of the image. This may be set to zero for BI_RGB bitmaps
			biSizeImage & 0xff,
			(biSizeImage >> 8) & 0xff,
			(biSizeImage >> 16) & 0xff,
			(biSizeImage >> 24) & 0xff,
			// LONG biXPelsPerMeter, unused
			0,
			0,
			0,
			0,
			// LONG biYPelsPerMeter, unused
			0,
			0,
			0,
			0,
			// DWORD biClrUsed, the number of color indexes of palette, unused
			0,
			0,
			0,
			0,
			// DWORD biClrImportant, unused
			0,
			0,
			0,
			0,
		];

		const iPadding = (4 - ((biWidth * 3) % 4)) % 4;

		const aImgData = oData.data;

		let strPixelData = "";
		const biWidth4 = biWidth << 2;
		let y = biHeight;
		const fromCharCode = String.fromCharCode;

		do {
			const iOffsetY = biWidth4 * (y - 1);
			let strPixelRow = "";
			for (let x = 0; x < biWidth; x++) {
				const iOffsetX = x << 2;
				strPixelRow +=
					fromCharCode(aImgData[iOffsetY + iOffsetX + 2]) +
					fromCharCode(aImgData[iOffsetY + iOffsetX + 1]) +
					fromCharCode(aImgData[iOffsetY + iOffsetX]);
			}

			for (let c = 0; c < iPadding; c++) {
				strPixelRow += String.fromCharCode(0);
			}

			strPixelData += strPixelRow;
		} while (--y);

		return (
			encodeData(BITMAPFILEHEADER.concat(BITMAPINFOHEADER)) +
			encodeData(strPixelData)
		);
	};

	/**
	 * saveAsImage
	 * @param canvas canvasElement
	 * @param width {String} image type
	 * @param height {Number} [optional] png width
	 * @param type {string} [optional] png height
	 * @param fileName {String} image name
	 */
	const saveAsImage = function (canvas, width, height, type, fileName) {
		// save file type
		const fileType = type;
		if ($support.canvas && $support.dataURL) {
			if (typeof canvas == "string") {
				canvas = document.getElementById(canvas);
			}
			if (type === undefined) {
				type = "png";
			}
			type = fixType(type);
			if (/bmp/.test(type)) {
				const data = getImageData(scaleCanvas(canvas, width, height));
				const strData = genBitmapImage(data);
				// use new parameter: fileType
				saveFile(makeURI(strData, downloadMime), fileType, fileName);
			} else {
				const strData = getDataURL(canvas, type, width, height);
				// use new parameter: fileType
				saveFile(strData.replace(type, downloadMime), fileType, fileName);
			}
		}
	};

	const convertToImage = function (canvas, width, height, type) {
		if ($support.canvas && $support.dataURL) {
			if (typeof canvas == "string") {
				canvas = document.getElementById(canvas);
			}
			if (type === undefined) {
				type = "png";
			}
			type = fixType(type);

			if (/bmp/.test(type)) {
				const data = getImageData(scaleCanvas(canvas, width, height));
				const strData = genBitmapImage(data);
				return genImage(makeURI(strData, "image/bmp"));
			} else {
				const strData = getDataURL(canvas, type, width, height);
				return genImage(strData);
			}
		}
	};

	return {
		saveAsImage: saveAsImage,
		saveAsPNG: function (canvas, width, height, fileName) {
			return saveAsImage(canvas, width, height, "png", fileName);
		},
		saveAsJPEG: function (canvas, width, height, fileName) {
			return saveAsImage(canvas, width, height, "jpeg", fileName);
		},
		saveAsGIF: function (canvas, width, height, fileName) {
			return saveAsImage(canvas, width, height, "gif", fileName);
		},
		saveAsBMP: function (canvas, width, height, fileName) {
			return saveAsImage(canvas, width, height, "bmp", fileName);
		},

		convertToImage: convertToImage,
		convertToPNG: function (canvas, width, height) {
			return convertToImage(canvas, width, height, "png");
		},
		convertToJPEG: function (canvas, width, height) {
			return convertToImage(canvas, width, height, "jpeg");
		},
		convertToGIF: function (canvas, width, height) {
			return convertToImage(canvas, width, height, "gif");
		},
		convertToBMP: function (canvas, width, height) {
			return convertToImage(canvas, width, height, "bmp");
		},
	};
})();

// Export function, used in npm
//export default Canvas2Image;