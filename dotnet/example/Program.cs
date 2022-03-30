﻿// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Logging;
using AzureGBB.AppDev.Pki.Services;
using AzureGBB.AppDev.Pki.Models.RSA;
using System.Security.Cryptography;

Console.WriteLine("Starting PKI Service");

using ILoggerFactory loggerFactory = LoggerFactory.Create(builder => 
{
	builder
		.AddConsole()
		.AddFilter("Microsoft", LogLevel.Debug)
		.AddFilter("System", LogLevel.Debug)
		.AddFilter("LoggingConsoleApp.Program", LogLevel.Debug);
});

String vaultName 	= "rkpkitest";
String keyName		= "rootCa";
String fqdn 			= "pki.raykao.dev";

AzureKeyVaultCertificateAuthority rootca = new AzureKeyVaultCertificateAuthority(
	logger: loggerFactory.CreateLogger<AzureKeyVaultCertificateAuthority>(), 
	azureKeyVaultName: vaultName, 
	keyName: keyName, 
	fqdn: fqdn
);

// Write Root Certificate to local file
byte[] rawRootCert = rootca.RootCertificate.RawData;
string rootCertPem = new string(PemEncoding.Write("CERTIFICATE", rawRootCert));
File.WriteAllText("rootCa.pem", rootCertPem);

// Create Client Cert
RSA rsa = RSA.Create();
RSAPublicKeyParameters publicKey = new RSAPublicKeyParameters(modulus: rsa.ExportParameters(false).Modulus, exponent: rsa.ExportParameters(false).Exponent);
string clientCert = rootca.IssueLeafCertificate("test.raykao.dev", publicKey);

// Write the client cert to local file
File.WriteAllText("clientCert.pem", clientCert);

// Note For ASP.NET sending rootCertificate
// content-type: application/x-pem-file
// File()

Console.WriteLine("PKI Complete.");
Console.WriteLine("You can now compare the ca.pem with the client.pem.");
Console.WriteLine("e.g.'openssl verify -verbose -CAfile rootCa.pem clientCert.pem' ");