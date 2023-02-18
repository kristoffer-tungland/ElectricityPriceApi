# Electrical Price API
This API provides access to day-ahead electrical price data for various regions.

## Usage
To use this API, send a GET request to the following endpoint:

```
https://electricitypriceapi.azurewebsites.net/api/Price?area={area}&fromDate={fromDate}&toDate={toDate}&currency={currency}&format={format}
```

### Input parameters:

{area}: The name of the area you are interested in.
{fromDate}: The starting date for the day-ahead prices in the format of YYYY-MM-DD.
{toDate}: The ending date for the day-ahead prices in the format of YYYY-MM-DD.
{currency}: The currency used to quote the price.
{format}: The response format, either json or xml.

The API will return a JSON or XML object with the following fields:

```json
{
  "currencyUnitName": "string",
  "priceMeasureUnitName": "string",
  "prices": [
    {
      "time": "2023-02-18T21:26:18.333Z",
      "price": 0
    }
  ]
}
```

## Authentication
This API does not require authentication.

## Rate Limiting
This API enforces rate limiting to prevent abuse. Requests are limited to 100 per hour per IP address. If you exceed this limit, you will receive a 429 error.

## Contributing
We welcome contributions to this API! If you find a bug or have an idea for an improvement, please open an issue or submit a pull request on GitHub.

## License
This API is licensed under the MIT License. See the LICENSE file for more information.