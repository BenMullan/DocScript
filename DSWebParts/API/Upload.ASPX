<%@ Page Language="VB" Debug="True" ValidateRequest="False" %> <%@ Import Namespace="DocScript.CompilerExtentions" %>
<%
	REM This page dosen't validate requests, because the DocScript source posted to it sometimes contains HTML-like structures.
	Global.DocScript.WebParts.API.UploadASPX(Request, Response)
%>