import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { BehaviorSubject } from 'rxjs/Rx';
import { HubConnectionBuilder } from '@aspnet/signalr';
import { FormControl, FormGroup } from '@angular/forms';

@Component({
    selector: 'reactiveChat',
    templateUrl: './reactiveChat.component.html'
})
export class ReactiveChatComponent implements  OnInit {
    private _hubConnection = new HubConnectionBuilder().withUrl('/hubs/chat').build();
    message = new FormControl();
    chatForm: FormGroup = new FormGroup({
        message: this.message
    });
    messages: string[]  = [];
    
    public sendMessage(text: string): void {
        const data = 'Sent: ' + text;
        this._hubConnection.invoke('Send', text);
        this.messages.push(data);
    }

    ngOnInit() {

        this.message.valueChanges
            .distinctUntilChanged()
            .debounceTime(500)
            .filter(t => t !== "foo")
            .subscribe(text => {
                this.sendMessage(text);
            });

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
