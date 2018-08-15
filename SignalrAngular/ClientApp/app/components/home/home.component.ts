import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { BehaviorSubject } from 'rxjs/Rx';
import { HubConnectionBuilder } from '@aspnet/signalr';

@Component({
    selector: 'home',
    templateUrl: './home.component.html'
})
export class HomeComponent implements  OnInit {
    private _hubConnection = new HubConnectionBuilder().withUrl('/hubs/chat').build();
    message: string = '';
    messages: string[]  = [];
    
    public sendMessage(): void {
        const data = 'Sent: ' + this.message;
        this._hubConnection.invoke('Send', this.message);
        this.messages.push(data);
    }
    ngOnInit() {
        this._hubConnection.on('Send', (data: any) => {
            const received='Received: ' + data;
            this.messages.push(received);
        });
        this._hubConnection.start()
            .then(() => {
                console.log('Hub connection started');
            })
            .catch(err => { console.log('Error establishing connection');}
        );
    }
}
