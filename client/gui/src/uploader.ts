import { fromEvent } from 'rxjs';
import { flatMap, takeUntil, map } from 'rxjs/operators';
import { createServer } from 'net';

console.log('from Uploader');

const server = createServer({ allowHalfOpen: true });
const connections = fromEvent(server, 'connection');

connections.pipe(
  //flatten the messages into their own Observable
  flatMap(
    (socket: any) => {
      return fromEvent(socket.__socket, 'message').pipe(
        //Handle the socket closing as well
        takeUntil(fromEvent(socket.__socket, 'close'))
      );
    },
    (socket, msg) => {
      //Transform each message to include the socket as well.
      return { socket: socket.__socket, data: msg };
    }
  ),
  map(msg => console.log(msg))
);

server.listen(5010);
