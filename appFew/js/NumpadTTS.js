
function showTTSNumpad(colorCells, colorNumbers,colorCellClean,colorTextClean,colorBorder,sizeButtons,sizeNumbersAndClear)
{
    try
	{
	    if (colorCells == null || colorCells =="")
	    {
	        colorCells = 'red';
	    }
	    if (colorNumbers == null || colorNumbers =="")
	    {
	        colorNumbers = 'white';
	    }
	    if (colorCellClean == null || colorCellClean =="")
	    {
	        colorCellClean = 'orange';
	    }
	    if (colorTextClean == null || colorTextClean =="")
	    {
	        colorTextClean = 'white';
	    }
	    if (colorBorder == null || colorBorder =="")
	    {
	        colorBorder = 'white';
	    }
	    
	    if (sizeButtons == null || parseInt(sizeButtons) < 50)
	    {
	        sizeButtons = '50';
	    }
	    if (sizeNumbersAndClear == null || parseInt(sizeNumbersAndClear) < 5)
	    {
	        sizeNumbersAndClear = '5';
	    }
	}
	catch (ex)
	{
	    colorCells = 'red';
	    colorNumbers = 'white';
	    colorCellClean = 'orange';
	    colorTextClean = 'white';
	    colorBorder = 'white';
	    sizeButtons = '50';
	    sizeNumbersAndClear = '5';
	}
	this.FormatCellsNumpad()
	this.CustomCellsNumpad(colorCells,colorNumbers,colorCellClean,colorTextClean,colorBorder,sizeButtons,sizeNumbersAndClear)
	
	for (var4= 0; var4< this.TotalCellsTable; var4++)
		document.getElementById(this.numpad_aux + var4).innerHTML = '&nbsp;&nbsp;'+this.array0[var4]+'&nbsp;&nbsp;';

}

function TTSNumpad(TTSNumpad_id)
{
	
	this.TotalCellsTable= 10;
	
	this.FormatCellsNumpad= FormatCellsNumpad;
	this.showTTSNumpad= showTTSNumpad;
	this.GetValueArray= GetValueArray;
	
	this.GetValueArrayAux= GetValueArrayAux;
	
	this.CustomCellsNumpad= CustomCellsNumpad
	this.CreateCellNumpad= CreateCellNumpad
	this.AlignCenter= AlignCenter
	this.CloseRow= CloseRow
	
    if (TTSNumpad_id == '')
		alert('TTSNumpad_id empty')
	
	this.numpad_aux= TTSNumpad_id
	
	this.name= TTSNumpad_id
}

function GetValueArray(j)
{
	return this.array0[j];
}
function GetValueArrayAux(j)
{
	return this.array2[j];
}

function FormatCellsNumpad()
{
	array1= new Array(this.TotalCellsTable);
	for (var4=0; var4<this.TotalCellsTable; var4++) array1[var4]=var4;

	this.array0= Array(this.TotalCellsTable);

	for (var4=this.TotalCellsTable-1; var4>=0; var4--)
	{
		index = Math.floor(Math.random() * var4);
		this.array0[var4]= array1[index]
		if ( index != var4 )
		array1[index]= array1[var4];
	}
}

function CreateCellNumpad(j, colorCells, colorNumbers, colorBorder,sizeNumbersAndClear) {
	but_id= this.numpad_aux+j;
	
	str= '\<td class="td1" style="font-family: Arial; background-color: '+colorCells+'; border-right: '+colorBorder+' 1px solid; border-top: '+colorBorder+' 1px solid; border-left: '+colorBorder+' 1px solid; border-bottom: '+colorBorder+' 1px solid;">\<font>\<a id="'+but_id+'" style="color: '+colorNumbers+'; text-decoration: none; font-size:'+sizeNumbersAndClear+'px" href="javascript:KeyPressed(\''+this.name+'\','+j+')">'+j+'\</a>\</font>\</td>'
	document.write(str)
}

function AlignCenter() {
	document.write('\<tr align="center">')
}

function CloseRow() {
	document.write('\</tr>')
}

function CustomCellsNumpad(colorCells, colorNumbers,colorCellClean,colorTextClean, colorBorder, sizeButtons,sizeNumbersAndClear) {
	document.write('\<table class="pinpad" cellspacing="0" cellpadding="0" style="width:'+sizeButtons+'px; height:'+sizeButtons+'px" >')

	for (var4=0; var4< this.TotalCellsTable; var4++) {
		if (var4%3==0) this.AlignCenter()
		this.CreateCellNumpad(var4, colorCells, colorNumbers, colorBorder,sizeNumbersAndClear) 
		if (var4%3==2) this.CloseRow()
	}

	document.write('\<td class="td2" colspan="2" style="font-family: Arial; font-weight: bold; background-color: '+colorCellClean+'; border-right: '+colorBorder+' 1px solid; border-top: '+colorBorder+' 1px solid; border-left: '+colorBorder+' 1px solid; border-bottom: '+colorBorder+' 1px solid;">\<font>\<a id="bclear" style="color: '+colorTextClean+'; text-decoration: none; font-size:'+sizeNumbersAndClear+'px" href="javascript:ClearPassword(\''+this.name+'\')">Limpiar\</a>\</font>\</td>');
	
	document.write('\</tr>\</table>');
}

function getTxtPassword(){
	var list = document.getElementsByTagName("input");
	for (var i=1;i<list.length;i++) {
		if(list[i].name.indexOf("pin")>0){
			return list[i];
		}
	}
}

function ClearTxtPassword(obj)
{   if(obj==null) obj=getTxtPassword(); 
	if (obj) obj.value= '';
}
function CellKeyPressed(obj, id, ocontrol, maxlength)
{	if(ocontrol==null) ocontrol=getTxtPassword();
	if(maxlength==null) maxlength=6;
	var1= getValue(obj, id);
	
	if (ocontrol && (ocontrol.value.length < maxlength) ) {
		ocontrol.value += var1;
	}	


}
function getValue(obj, id)
{
	return eval( obj + '.GetValueArray(' +id+ ');' )
}

function getValueAux(obj, id)
{
	return eval( obj + '.GetValueArrayAux(' +id+ ');' );
}