<%@ Page Language="VB" %> <%@ Import Namespace="DocScript.CompilerExtentions" %>
<!-- Ben Mullan (c) 2023. All Rights Reserved. -->
<html>
	<head>

		<title>DocScript: Interactive!</title>
		<!-- <meta charset="UTF-8" /> -->
		<meta name="description" content="DSI Home" />
		<meta name="keywords" content="DocScript, HomePage" />
		<meta name="author" content="Ben Mullan" />
		<meta name="viewport" content="width=device-width, initial-scale=0.9" />
		
<%= Response.GetScriptAndCSSImports().TabbedInBy(2) %>
		
	</head>
	<body onload="">
			
		<!-- Site Title & NavBar -->
		<div class="container text-center">

			<!-- DSInteractive Icon -->
			<img src="/ClientPages/Resources/Images/DSInteractive.PNG" class="img-fluid me-5 mt-3" width="8%" alt="DocScript Interactive Logo" />

			<!-- Site Title -->
			<a class="ds-title display-4 text-decoration-none" href="/" target="_self">DocScript Interactive</a>
		
			<!-- NavigationBar -->
			<div class="container justify-content-center">
				<a class="px-4 justify-content-center" href="/ClientPages/UploadProgram.ASPX" target="_self">Upload Program</a>
				<a class="px-4 justify-content-center" href="javascript: window.location.href = ('/ClientPages/Interactive/ESParticipant.ASPX?Role=Observer&ESID=' + encodeURIComponent(window.prompt(`\r\nExecution-Sessions can be created and managed via &quot;Manage Sessions&quot;.\r\nEach Session has a unique ID called an ESID.\r\n\r\nEnter an Execution-Session-ID...`)));" target="_self">Join Session</a>
				<a class="px-4 justify-content-center" href="/ClientPages/Interactive/ESManager.ASPX" target="_self">Manage Sessions</a>
				<a class="px-4 justify-content-center" href="/" target="_self">Help!</a>
			</div>
			
			<hr/>

		</div>
		
		<!-- Main page content -->
		<main role="main" class="container">

			<!-- Page Heading -->
			<div class="h2 ds-heading">DSI: Home</div>
			<div class="h4 ds-heading">Welcome to DocScript Interactive!</div>
			<br/>

			<!-- The YouTube Video -->
			<iframe width="560" height="315" src="https://www.youtube.com/embed/ybl5pVSJOOk" title="DS3Min YouTube Video" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share" allowfullscreen="true"></iframe>
			
			<p class="py-4">
				<b>DSI enables the execution of DocScript Programs in Real-Time, Multi-Client, Execution-Sessions.</b>
				<br/><br/>
				Participants of an Execution-Session have one of three possible roles; Observer, Responder, or Controller.<br/>
				Observers, can watch the Session's Events occur.<br/>
				Responders, can also respond to Input-Requests from the Program (caused by the DS Input() Function).<br/>
				Controllers, can also inject JavaScript "Client-Execution-Packages", which are immediately executed on all clients of the Session. 
				Controllers also have the ability to re-initiate the session when it is over.
				<br/><br/>
				<i>To get started, use the links in the Navigation-Bar...</i>
			</p>

			<p class="py-4">
				<div class="h4 ds-heading">Debugging Links</div>
				<ul>
					<li><a href="/ClientPages/Useful_CEPs.JS">Useful CEPs</a></li>
					<li><a href="/ClientPages/Interactive/ESParticipant.ASPX?Role=Controller&ESID=FINITE_66G&AutoRejoin=True">Join Mass-testing CEPClient...</a></li>
				</ul>
			</p>
														
		</main>

		<!-- Footer. **ToDo: Make sticky to bottom of page -->
		<div class="container mt-5 pt-5">
			<footer class="d-flex flex-wrap justify-content-between align-items-center py-3 my-4 border-top">
		
				<div class="col-md-6 d-flex align-items-center">
					<span class="mb-3 mb-md-0 text-muted"><strong class="pe-4">DocScript: Interactive</strong>&copy; Ben Mullan 2023<em class="ps-4"><a href="https://cookieconsentspeed.run/">Cookies what?</a></em></span>
				</div>

				<!-- Social Medium Link -->
				<ul class="nav col-md-4 justify-content-end list-unstyled d-flex">
					<li class="ms-3" title="BM Homepage"><a class="text-muted bi-person-square" href="http://benm.eu5.org/" target="_blank"></a></li>
				</ul>
		
			</footer>
		</div>

	</body>
</html>
<% Response.SafelyEnd() %>