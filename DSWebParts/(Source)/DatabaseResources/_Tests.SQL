DECLARE @TheProgramName VARCHAR(100); SET @TheProgramName = 'HelloWorld.DS';
DECLARE @TheSource NVARCHAR(MAX); SET @TheSource = 'Function<Void>Main()' + CHAR(13)+CHAR(10) + 'Output("Hello, World!")' + CHAR(13)+CHAR(10) + 'EndFunction';
DECLARE @TheTimeUploaded DATETIME; SET @TheTimeUploaded = CURRENT_TIMESTAMP;

INSERT INTO [DSInteractive_DSDev].[dbo].[UploadedPrograms] ("ProgramName", "Source", "TimeUploaded") VALUES (@TheProgramName, @TheSource, @TheTimeUploaded);