<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="launch.aspx.cs" Inherits="appFew.launch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />

    <title>Incatops</title>

        <link href="~/Content/swagg.css" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="~/Content/LloydsHbkUy.css" />
    <script src="Scripts/jquery-1.10.2.js"></script>

    <script src="Scripts/actions.js"></script>
    <script src="Scripts/jquery.mCustomScrollbar.js"></script>
</head>
<script type ="text/javascript" language="javascript">
        var newwindow;
		function pop_cajaarequipa(url)
		{
		    //var ancho = window.innerWidth; //screen.Width;
		    //var alto = screen.Height;
            var ancho = screen.availWidth - 15; //Math.max(document.documentElement.clientWidth, window.innerWidth || 0);
            var alto = screen.availHeight - 60; //Math.max(document.documentElement.clientHeight, window.innerHeight || 0)		    ;
		    var izquierda = 0; //(screen.availWidth -800) / 2; 
            var arriba = 0; //(screen.availHeight - 600) / 2; 
            newwindow = window.open(url, 'Caja Arequipa', 'width=' + ancho + ',height=' + alto + ',resizable=no,scrollbars=1,status=yes,locationbar=no,menubar=no,location=no,toolbar=no,titlebar=Yes,left=' + izquierda + ',top=' + arriba);
			if (window.focus) {newwindow.focus()}
		}

    </script>
    
<body>
    <form id="form1" runat="server">
    <div>
    <span >
        <a href="javascript:pop_cajaarequipa('Login.aspx');">Accede</a></span>    
    </div>
    </form>
</body>
</html>
