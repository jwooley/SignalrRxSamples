import { Component, OnInit } from '@angular/core';
import { distinctUntilChanged, debounceTime, filter } from 'rxjs/operators';
import { HubConnectionBuilder } from '@microsoft/signalr';
import { FormControl, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-reactive-chat',
  templateUrl: './reactive-chat.component.html'
})
export class ReactiveChatComponent implements OnInit {
  public hubUrl: string = "";
  _hubConnection: any;
  constructor() {
    this.hubUrl = 'https://localhost:7183/hubs/chat';
  }
  message = new FormControl();
  chatForm = new FormGroup({
        message: this.message,
      });
  messages: string[] = [];

  public sendMessage(text: string): void {
    const data = 'Sent: ' + text;
    this._hubConnection.invoke('Send', text);
    this.messages.push(data);
  }
  public send(): void {
    const text: string = (<HTMLInputElement>document.getElementById('message')).value;
    this.sendMessage(text);
  }
  ngOnInit() {
    this._hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl)
      .build();

    this._hubConnection.start()
      .then(() => {
        console.log('Hub connection started');
        this._hubConnection.on('Send', (data: any) => {
          const received = 'Received: ' + data;
          this.messages.push(received);
        });
      })
      .catch((err: any) => { console.log('Error establishing connection' + err); }
    );




    // RxJS 1
    //this.message.valueChanges
    //  .distinctUntilChanged()
    //  .debounceTime(500)
    //  .where(t => t !== 'foo')
    //  .select(t => t);

    // Rxjs 6 and later
    this.message.valueChanges
      .pipe(
        distinctUntilChanged(),
        debounceTime(500),
        filter(t => t !== 'foo')
      ).subscribe(text => {
        this.sendMessage(text);
      });
  }
}
