/* DSI */
DSI.MsgBox("Hello!");
DSI.Freeze();

/* BSOD */
document.write(`<img src="/ClientPages/Resources/Images/BSOD.PNG" style="height: 100%; width: 100%; position: fixed; top: 0; right: 0;" />`);

/* BSOD - append() */
$("hr").hide();$("body").prepend(`<img src="/ClientPages/Resources/Images/BSOD.PNG" style="height: 100%; width: 100%; position: fixed; top: 0; right: 0;" />`);$("body").css("overflow", "hidden");

/* Nice-Music */
$("body").append(`<audio id="AudioPlayer" preload="auto" loop="true"><source src="/ClientPages/Resources/Audio/ShortFlowerDuet.MP3" type="audio/mpeg"/></audio>`); $("#AudioPlayer").get(0).play();

/* Emetic Background */
$("body").css("background-image", `url("/ClientPages/Resources/Images/Loading.GIF")`);

/* GetLatestImage */
var LatestImage_Count = 0;
function GetLatestImage() {
	if ($("#LatestImage").length == 0) {
		$("hr").hide();
		$("body").prepend(`<img id="LatestImage" src="/ClientPages/Resources/Images/LatestImage.PNG" style="height: 100%; width: 100%; position: fixed; top: 0; right: 0;" />`);
		$("body").css("overflow", "hidden");
	}
	$("#LatestImage").attr("src", "/ClientPages/Resources/Images/LatestImage.PNG?RequestCount=" + (LatestImage_Count++).toString());
	setTimeout(GetLatestImage, 1000);
}