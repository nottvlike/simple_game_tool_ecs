rm -r ./Protocol
rm ./Protocols.cs

./flatc --lua ./Login.fbs ./Protocols.fbs ./Item.fbs