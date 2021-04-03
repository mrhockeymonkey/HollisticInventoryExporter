# HIE - Hollistic Inventory Exporter
An example tool for exporting seperate inventories into a single source.

Inventory owners are expected to export their data into a parseable format and apply
pre-built validations by specififying them as settings. The also acts as the exporter
by hosting data on a web endpoint to be scraped (akin to prometheus)

## Usage
```bash
# view command help and/or version
hie.exe --help
hie.exe --version

# show human readable information on data validity
hie.exe validate --file data.json

# publish data to be scraped
hie.exe serve --file data.json
```

## Build
```bash
dotnet publish -r win-x64 -p:PublishSingleFile=true --self-contained true
```
