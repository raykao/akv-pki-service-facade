namespace AzureGBB.AppDev.Pki.Tests;

using AzureGBB.AppDev.Pki.Services;
using AzureGBB.AppDev.Pki.Models.RSA;
using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Logging;
using Xunit;


public class RootCaContextFixture : IDisposable
{
	public AzureKeyVaultCertificateAuthority RootCa { get; private set; }
	public string LeafCertificate { get; private set; }
	private string _vaultName 	= "rkpkitest";
	private string _rootKeyName		= "rootCa";
	private string _rootFqdn			= "pki.raykao.dev";
	private string leafSubjectName = "test.raykao.dev";

	public RootCaContextFixture()
	{
		ILoggerFactory loggerFactory = LoggerFactory.Create(builder => 
		{
			builder
				.AddConsole()
				.AddFilter("Microsoft", LogLevel.Debug)
				.AddFilter("System", LogLevel.Debug)
				.AddFilter("LoggingConsoleApp.Program", LogLevel.Debug);
		});

		RootCa = new AzureKeyVaultCertificateAuthority(
			logger: loggerFactory.CreateLogger<AzureKeyVaultCertificateAuthority>(), 
			azureKeyVaultName: _vaultName, 
			keyName: _rootKeyName, 
			fqdn: _rootFqdn
		);

		RSAParameters leafRsa = RSA.Create().ExportParameters(false);
		RSAPublicKeyParameters leafPublicKeyParameters = new RSAPublicKeyParameters(modulus: leafRsa.Modulus, exponent: leafRsa.Exponent);
		
		LeafCertificate = RootCa.IssueLeafCertificate(leafSubjectName, leafPublicKeyParameters);
	}

	public void Dispose()
	{
		// Clean up: Delete RootCa Key/Cert in Azure Key Vault
	}

}

public class AzureKeyVaultCertificateAuthorityTests : IClassFixture<RootCaContextFixture> {
	
	AzureKeyVaultCertificateAuthority RootCa;

	public AzureKeyVaultCertificateAuthorityTests(RootCaContextFixture fixture) {
		this.RootCa = fixture.RootCa;
	}

	[Fact]
	public void RootCertificate_ShouldReturnX509Cert()
	{
		// Arrange
		// var expected = X509Certificate2 Class

		// Act
		
		var actual = RootCa.RootCertificate;

		// Assert
		Assert.IsType<X509Certificate2>(actual);
	}

	[Fact]
	public void IssueLeafCertificate_ShouldReturnX509Cert()
	{
		// Arrange

		// Act

		// Assert
	}

	[Fact]
	public void IssueLeafCertificate_ShouldHaveSameIssuerAsRootCa()
	{

	}
}