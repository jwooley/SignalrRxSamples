import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { BehaviorSubject } from 'rxjs/Rx';
import { HubConnection } from '@aspnet/signalr';

@Component({
    selector: 'sensor',
    templateUrl:'./sensor.component.html'
})
export class SensorComponent implements OnInit {
    private _hubConnection = new HubConnection('/hubs/sensor');
    dataStream: SensorData[] = [];
    subject: BehaviorSubject<SensorData> = new BehaviorSubject<SensorData>(new SensorData());

    ngOnInit() {
        this.subject
            .filter(val => val.sensorType === '1')
            .subscribe({
                next: (value: SensorData) => this.dataStream.push(value),
                error: function (err) { console.log(err); },
                complete: () => console.log("done")
            });

        this._hubConnection.start()
            .then(() => {
                var obs = this._hubConnection.stream<SensorData>("Values").subscribe({
                    next: val => this.subject.next(val),
                    error: err => this.subject.error(err),
                    complete: () => this.subject.complete()
                });
            });
    }
}

class SensorData {
    timeStamp: Date;
    sensorType: string;
    sensorValue: number;
}
