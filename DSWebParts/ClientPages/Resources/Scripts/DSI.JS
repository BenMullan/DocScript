/*
	The DSI Object is for use in CEPs.
*/

window.DSI = {
	MsgBox: function (_MessageText) {
		Swal.fire(_MessageText);
	},
	Freeze: function () {
		document.title = "(Frozen)";
		document.write();
		debugger;
	},
	TimeTitle: function () {
		setInterval(function () { document.title = (new Date()).toTimeString(); }, 1000);
	}
};