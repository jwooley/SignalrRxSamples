import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { BehaviorSubject } from 'rxjs/Rx';
import { HubConnection } from '@aspnet/signalr-client';

@Component({
    selector: 'sensor',
    templateUrl:'./sensor.component.html'
})
export class SensorComponent implements OnInit {
    private _hubConnection: HubConnection;
    dataStream: SensorData[] = [];
    subject: BehaviorSubject<SensorData> = new BehaviorSubject<SensorData>(new SensorData());

    ngOnInit() {
        this._hubConnection = new HubConnection('/hubs/sensor');

        this.subject
            .subscribe({
                next: (value: SensorData) => this.dataStream.push(value),
                error: function (err) { console.log(err); },
                complete: () => console.log("done")
            });

        this._hubConnection.start()
            .then(() => {
                //var obs = <Observable<SensorData>>this._hubConnection.stream("Values");
                //obs.filter(val => val.SensorValue < 7)
                this._hubConnection.stream("Values")
                    .filter(value => value.SensorValue < 7)
                    .subscribe(this.subject);
            });
    }
}

class SensorData {
    TimeStamp: Date;
    SensorType: string;
    SensorValue: number;
}
