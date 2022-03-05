# Azure Key Vault Public Key Infrastructure Service Facade

This project is an example app meant for demonstrating how to use Azure Key Vault (AKV) as a Root Certificate Authority (Root CA) for a self managed Public Key Infrastructure Service (PKI).  This example app acts as a facade on top/in front of AKV and provides the expected interface/capabilities that a PKI service is expected to provide.

## Why

Azure Key Vault does not today provide the ability to act as a Root CA out of the box.  In many cases organizations want the capability to centrally issue X509 certificates for encrypted communication between services (e.g. TLS).  There is often also a need/want to leverage a mutual authentication mechinsim to verify each communicating party (client and server).  Today this second concern is usually provided by mutual TLS (mTLS) where both parties (client and server) each have a TLS certificate signed by the same central authority (Root CA).  AKV is a perfect service/method to host the Root CA (public certificate and private key) but it lacks the ability to act as a full PKI service.  As such this repo/project is designed to show how one could implement a PKI service on top of AKV - providing the features of a PKI that AKV does not support/provide today.

## Goals
- Bootstrap/generate a Root CA (self signed public Root certificate and a private Root CA Key) and host/store it in AKV
- Generate the Root CA entirely in AKV; Never expose/save Root CA pub/private key to the local app/server
	- We want to limit the possible exposure of the Private Key, hence only have it in AKV and disable the ability to export the Private Key
- Provide the ability to submit a Common Name (CN) and a public certificate to be signed by AKV
- Provide a way to validate the requestor (AAD/HTTP/DNS) for the given App Identity (typically a FQDN)
- Use 

## PKI Workflow
1. Requestor self generates a Public Certificate/Private Key Pair (typically in their local environment)
	- Example: A Web Server will generate this in their local environment
2. Requestor will send a Certificate Signing Request to the PKI service OR simply send the CN + Public Certificate
3. The PKI app will validate the identity of the Requestor (multiple possible validation methods)
4. The PKI app will send a CSR request to AKV
5. The PKI app will return the Signed CSR

## Notes
- AKV can generate self-signed certificates via the Azure Portal, SDKs and REST APIs.  However it does not expose the ability to make these certificates as Root CA via Portal and SDKs (specifically dotnet/C#).  The REST API however does and so in the app implementations for generating a Root CA (signing certificate) we must use an authenticated REST call to create/generate the Root CA instead of using the native SDK(s)

## Questions/Issues
- Should we be using IConfiguration and inject config object to get settings values?
- Should we revert to v3 of the Azure-sdk-for-dotnet (Microsoft.Azure.KeyVault)? Specifically for the creation of the Self-Signed CA Cert in AKV?
- Use of a ```JsonConverter``` for UTC time on ```models.attribtes``` base class?

## Initial Design Diagram

![Initial design diagram](/img/initial-diagram.png)