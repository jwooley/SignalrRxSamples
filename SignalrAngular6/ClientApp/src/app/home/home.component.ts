import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { HubConnectionBuilder } from '@aspnet/signalr';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html'
})
export class HomeComponent implements OnInit {
  private _hubConnection = new HubConnectionBuilder().withUrl('/hubs/chat').build();
  message = '';
  messages: string[] = [];

  public sendMessage(): void {
    const data = 'Sent: ' + this.message;
    this._hubConnection.invoke('Send', this.message);
    this.messages.push(data);
  }
  ngOnInit() {
    this._hubConnection.on('Send', (data: any) => {
      const received = 'Received: ' + data;
      this.messages.push(received);
    });
    this._hubConnection.start()
      .then(() => {
        console.log('Hub connection started');
      })
      .catch(err => { console.log('Error establishing connection'); }
      );
  }
}
