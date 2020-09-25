# 4Insight Data Analytics Reservoir Connector sample

The sample demonstrate how to upload timestamp/value data in CSV format and adding that data to an existing timeseries.

## Configuration
Authentication is using the OAuth2 client credential flow, and requires a client id and a secret. These values must be set in the `appsettings.json` configuration file.

## Code
The `App` class is the main entry for the sample logic. The `Features` namespace contain logic for working with the various 4Insight APIs.
