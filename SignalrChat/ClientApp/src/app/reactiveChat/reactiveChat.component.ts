import { Component, OnInit } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';

@Component({
  selector: 'app-reactive-chat',
  templateUrl: './reactiveChat.component.html'
})
export class ReactiveChatComponent implements OnInit {
    _hubConnection: HubConnection;
    goodMessage: string;
    badMessage: string;
    goodMessages: string[] = [];
    badMessages: string[] = [];

  public sendGoodMessage(text: string): void {
    const data = 'Sent: ' + text;
    this._hubConnection.invoke('SendGood', text);
    this.goodMessages.push(data);
    }

  public sendGood(): void {
    this.sendGoodMessage(this.goodMessage);
    }

    public sendBadMessage(text: string): void {
        const data = 'Sent: ' + text;
        this._hubConnection.invoke('SendBad', text);
        this.badMessages.push(data);
    }

    public sendBad(): void {
        this.sendBadMessage(this.badMessage);
    }

  ngOnInit() {
    this._hubConnection = new HubConnectionBuilder().withUrl('/hubs/chat').build();

    this._hubConnection.start()
      .then(() => {
        console.log('Hub connection started');
        this._hubConnection.on('SendGood', (data: any) => {
          const received = 'Received: ' + data;
          this.goodMessages.push(received);
        });
          this._hubConnection.on('SendBad', (data: any) => {
              const received = 'Received: ' + data;
              this.badMessages.push(received);
          })
      })
      .catch(err => { console.log('Error establishing connection'); }
      );
  }
}
