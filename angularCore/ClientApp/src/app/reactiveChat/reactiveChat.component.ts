import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { distinctUntilChanged, debounceTime, filter } from 'rxjs/operators';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { FormControl, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-reactive-chat',
  templateUrl: './reactiveChat.component.html'
})
export class ReactiveChatComponent implements OnInit {
  _hubConnection: HubConnection;
  chatForm: FormGroup;
  message = new FormControl();
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
    this._hubConnection = new HubConnectionBuilder().withUrl('/hubs/chat').build();
    this.chatForm = new FormGroup({
      message: this.message,
    });

    //this.message.valueChanges
    //  .pipe(
    //    distinctUntilChanged(),
    //    debounceTime(500),
    //    filter(t => t !== 'foo')
    //  ).subscribe(text => {
    //    this.sendMessage(text);
    //  });


    this._hubConnection.start()
      .then(() => {
        console.log('Hub connection started');
        this._hubConnection.on('Send', (data: any) => {
          const received = 'Received: ' + data;
          this.messages.push(received);
        });
      })
      .catch(err => { console.log('Error establishing connection'); }
      );
  }
}
