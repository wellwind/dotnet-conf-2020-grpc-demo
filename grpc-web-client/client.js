const { HelloRequest, GetPriceRequest } = require('./greet_pb.js');
const { GreeterClient } = require('./greet_grpc_web_pb.js');

var client = new GreeterClient('http://localhost:9090');

var request = new HelloRequest();
request.setName('World');

client.sayHello(request, {}, (err, response) => {
  if (err) {
    console.error(err);
  }
  console.log(response.toObject());
  console.log(response.getMessage());
});

var priceRequest = new GetPriceRequest();
priceRequest.setStockid('2330');

const getStockPriceStream = client.getStockPrices(priceRequest, {});
getStockPriceStream.on('data', (response) => {
  console.log(response.toObject());
  console.log(response.getStockid());
  console.log(response.getPrice());
});
getStockPriceStream.on('error', (error) => {
  console.log(error);
});
