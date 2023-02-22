<%@ Page Language="VB" %> <%@ Import Namespace="DocScript.CompilerExtentions" %>
<%
	REM This file only exists so that the VS-Dev-Server can correctly debug the /API/Interactive/ Path...
	Global.DocScript.WebParts.API.ExecutionSessionASPX(Request, Response)
%>