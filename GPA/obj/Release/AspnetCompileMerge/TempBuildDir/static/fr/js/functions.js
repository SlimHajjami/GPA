
$(document).ready(function(){

menu();


});




function menu(){
	$(".pictomenu").click(function(){
	var m = $(this).next();
	if(m.is(":hidden")){
        m.slideDown();
		$(this).addClass("active");
		} 
		 else {
		m.slideUp();
		$(this).removeClass("active");
		
		
			 }});
 }