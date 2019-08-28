import { fromEvent, Observable } from 'rxjs';
import { flatMap, takeUntil, map } from 'rxjs/operators';
import { createServer, Server, Socket } from 'net';

export interface TcpOutput {
  socket: Socket;
  message: string;
}

export enum SocketStatus {
  'ready',
  'data',
  'end',
  'error',
  'drain',
  'close'
}

export interface ClientAddress {
  host: string;
  port: string;
}

export interface ConnectedClient {
  clientAddress: ClientAddress;
  data$: Observable<TcpOutput>;
  send(message: string, encoding?: string): Boolean;
  clientStatus$: Observable<SocketStatus>;
}

export class TcpConnection {
  private server: Server;
  public connection$: Observable<Socket>;
  public data$: Observable<TcpOutput>;

  constructor() {
    this.server = createServer({
      allowHalfOpen: true
    });

    this.connection$ = fromEvent(this.server, 'connection');

    this.data$ = this.connection$.pipe(
      flatMap(
        socket => {
          socket.setEncoding('utf8');
          return fromEvent(socket, 'data').pipe(
            //Handle the socket closing as well
            takeUntil(fromEvent(socket, 'end'))
          );
        },
        (socket, message): TcpOutput => ({
          socket: socket,
          message: message as string
        })
      )
    );
  }

  public listen(port: Number) {
    if (this.server && this.server.listening) {
      throw new Error('This server is already listening on ' + this.getAddress(this.server));
    }

    this.server.listen(port);
  }

  private getAddress(server: Server) {
    if (server === undefined || server === null || server.listening === false) return null;
    const address = server.address();
    if (typeof address === 'string') return address;
    return `${address.address}:${address.port}`;
  }
}
