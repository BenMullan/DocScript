'''<summary>
'''This is a test class for CollectionTypeExtentionsTests and is intended
'''to contain all CollectionTypeExtentionsTests Unit Tests
'''</summary>
<TestClass()>
Public Class CollectionTypeExtentions_Tests

#Region "Additional test attributes"
	'
	'You can use the following additional attributes as you write your tests:
	'
	'Use ClassInitialize to run code before running the first test in the class
	'<ClassInitialize()>  _
	'Public Shared Sub MyClassInitialize(ByVal testContext As TestContext)
	'End Sub
	'
	'Use ClassCleanup to run code after all tests in a class have run
	'<ClassCleanup()>  _
	'Public Shared Sub MyClassCleanup()
	'End Sub
	'
	'Use TestInitialize to run code before running each test
	'<TestInitialize()>  _
	'Public Sub MyTestInitialize()
	'End Sub
	'
	'Use TestCleanup to run code after each test has run
	'<TestCleanup()>  _
	'Public Sub MyTestCleanup()
	'End Sub
	'
#End Region

	'''<summary>A test for GenerateUniqueString()</summary>
	<TestMethod()>
	Public Sub GenerateUniqueStringTest()

		Dim _StringArray() As String = {"PROGRAM"}
		Dim _Seed As String = "PROGRAM"
		Dim expected As String = "PROGRA0"
		Dim actual As String =
		   DocScript.CompilerExtentions.CollectionTypeExtentions.GenerateUniqueString(_StringArray, _Seed)

		Assert.AreEqual(expected, actual)

	End Sub

End Class