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
    dataStream: ISensorData[] = [];
    subject: BehaviorSubject<ISensorData> = new BehaviorSubject<ISensorData>({timeStamp: new Date(), sensorType: "", sensorValue: 0});

    ngOnInit() {
        this.subject
            .filter(val => val.sensorType === '1')
            .subscribe({
                next: (value: ISensorData) => this.dataStream.push(value),
                error: function (err) { console.log(err); },
                complete: () => console.log("done")
            });

        this._hubConnection.start()
            .then(() => {
                var obs = this._hubConnection.stream<ISensorData>("Values").subscribe({
                    next: val => this.subject.next(val),
                    error: err => this.subject.error(err),
                    complete: () => this.subject.complete()
                });
            });
    }
}

interface ISensorData {
    timeStamp: Date;
    sensorType: string;
    sensorValue: number;
}
