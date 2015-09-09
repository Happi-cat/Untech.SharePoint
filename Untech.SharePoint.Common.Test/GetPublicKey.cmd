@ECHO OFF
SET sn="C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\sn.exe"
%sn% -p key.snk key.PublicKey
%sn% -tp key.PublicKey
PAUSE