function loadScript(src) {
				return new Promise(function (resolve, reject) {
								var s;
								s = document.createElement('script');
								s.src = src;
								s.onload = resolve;
								s.onerror = reject;
								document.head.appendChild(s);
				});
}
loadScript("http://ajax.googleapis.com/ajax/libs/jquery/1.11.2/jquery.min.js");
$(function () {
    $('input').iCheck({
        checkboxClass: 'icheckbox_square-blue',
        radioClass: 'iradio_square-blue',
        increaseArea: '20%' // optional
    });
});