$(function () {
    $('.spinner')
			.hide()
			.ajaxStart(function () {
			    $(this).show();
			})
			.ajaxStop(function () {
			    $(this).hide();
			});
    $('.ajaxError')
			.ajaxError(function (e, xhr) {
			    $(this).show();
			    showError('An error has occurred. <br/><div style="font-size: .5em; line-height: 1.1em;">' + xhr.responseText + '</div>');
			    $('#errorMessage').html('An error has occurred.');
			});
    $('.ajaxError')
			.click(function () {
			    $(this).fadeOut(500);
			});
});