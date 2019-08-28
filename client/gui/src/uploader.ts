import { TcpConnection } from './network/tcpConnection';

console.log('from Uploader');

var con = new TcpConnection();
con.data$.subscribe(
  next => console.log(next.message),
  () => console.log('error'),
  () => console.log('complete')
);
con.listen(5010);
