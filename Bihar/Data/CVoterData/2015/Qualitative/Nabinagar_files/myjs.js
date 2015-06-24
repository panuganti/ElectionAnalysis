j=$.noConflict(true);
$=j;
j(document).ready(function () {

                 //j("table").stupidtable();
                  j( document ).tooltip();

                  j("table.development").tablesorter({ 
				       sortList: [[0,0]] 
				  }); 

				   j('input').change(function(){
                 	colorme(j(this).val());
                 
                 });

				   	 j('.print').click(function(){ 
				   	 	window.print();                 
                 });

				   	

				   	  j('.closeme').click(function(){ 
				   	 		j(this).parent().fadeOut('show');
				   	 });

				    j( ".top3pm" ).draggable();
				    j( ".top3cm" ).draggable();
				   
								  
					
					  


					   
					   
				//	j('#headingmain').html("Odisha / "+j('#ContentPlaceHolder1_acname').html());
					
				/*	if(j('#ContentPlaceHolder1_acname').html()=="N/A") j('#headingmain').html("Odisha State Level Dashboard"); */
					j('.dispquestion').click(function(){
						
						j('.overlay').fadeIn();
						j('.qcontainer').fadeIn();
						j('.mcontainer').hide();
						j('.rcontainer').hide();
						j('.rframe').attr('src','wait.html');
								
					})

					j('.dispmethod').click(function(){
						
						j('.overlay').fadeIn();
						j('.mcontainer').fadeIn();
						j('.qcontainer').hide();
						j('.rcontainer').hide();
						j('.rframe').attr('src','wait.html');
					
					})

					j('.aclist').click(function () {
					 //   alert();
						j('.overlay').fadeIn();
						j('.divacs').slideDown();
					
					})


					j('.overlay').click(function(){
						j('.overlay').hide();
						j('.qcontainer').hide();
						j('.data-container').hide();
						j('.divacs').hide();
					   
					})
					

					j('.close').click(function () {
					    j('.overlay').hide();
					    j('.divacs').hide();
					});

					j('.close1').click(function(){
						j('.overlay').hide();
						j('.qcontainer').hide();
						j('.psephometer').hide();
						j('.psephometer1').hide();
						j('.debriefing-container').hide();
						j('.data-container').hide();
						j('.chart-container').hide();						
						j('.quickinfo').show();
						j('.gridddetails').show();	
						j('.mcontainer').hide();
						j('.rcontainer').hide();
						j('.rframe').attr('src','wait.html');
					})	

					

					j('.sample').click(function(){
						
						j('.samplesize').show();	
					})					
                	
});
				

            


function dbrf()
{
	vsno=j('#ContentPlaceHolder1_vno').html();
		j('.overlay').show();
		j('.debriefing-container').show();
		j('.frame').attr('src','html/'+vsno+'.pdf');
						
}

function printDiv(divID) {
           
            var iBody = j('.frame').contents().find("body").html();
          //  alert(iBody);
            
            //Get the HTML of whole page
            var oldPage = document.body.innerHTML;

            //Reset the page's HTML with div's HTML only
            document.body.innerHTML = 
              "<html><head><title></title></head><body>" + 
              iBody + "</body>";

            //Print Page
            window.print();

            //Restore orignal HTML
            document.body.innerHTML = oldPage;          
}