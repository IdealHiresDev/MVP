$(document).ready(function() {
	$('#list').click(function(event){
		event.preventDefault();
		$('#products .item').addClass('list-group-item');
		$(this).addClass('active');
		$('#grid').removeClass('active');
	});
	$('#grid').click(function(event){
		event.preventDefault();
		$('#products .item').removeClass('list-group-item');
		$('#list').removeClass('active');
		$(this).addClass('active');
		$('#products .item').addClass('grid-group-item');
		});
		$(".navbar-toggler").click(function(){
$(this).toggleClass("close");
});
});