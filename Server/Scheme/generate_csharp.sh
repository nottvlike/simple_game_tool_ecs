rm -r ./Protocol
rm ./Protocols.lua

./flatc --csharp ./Login.fbs ./Protocols.fbs ./Item.fbs ./System.fbs