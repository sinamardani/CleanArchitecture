# JWT Keys Directory

This directory contains the ECDSA keys for JWT token signing and validation.

## Setup Instructions

1. Generate ECDSA keys using one of the following methods:

### Method 1: Using OpenSSL (Recommended)
```bash
# Generate private key
openssl ecparam -genkey -name prime256v1 -noout -out private-key.pem

# Generate public key from private key
openssl ec -in private-key.pem -pubout -out public-key.pem
```

### Method 2: Using .NET Code
Create a console application and run:
```csharp
using System.Security.Cryptography;

var ecdsa = ECDsa.Create(ECCurve.NamedCurves.nistP256);
var privateKey = ecdsa.ExportECPrivateKeyPem();
var publicKey = ecdsa.ExportSubjectPublicKeyInfoPem();

Console.WriteLine("Private Key:");
Console.WriteLine(privateKey);
Console.WriteLine("\nPublic Key:");
Console.WriteLine(publicKey);
```

## Security Notes

- **NEVER** commit actual private keys to the repository
- This directory is already added to `.gitignore`
- For production, use Azure Key Vault, AWS Secrets Manager, or Environment Variables
- Keep private keys secure and never share them

## File Structure

- `private-key.pem` - ECDSA private key (ES256 algorithm)
- `public-key.pem` - ECDSA public key (ES256 algorithm)
- `.gitkeep` - Keeps the directory in git

