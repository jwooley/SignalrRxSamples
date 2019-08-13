
import {filter} from 'rxjs/operators';
import { Component, OnInit } from '@angular/core';
import { Observable ,  BehaviorSubject } from 'rxjs';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';

@Component({
    selector: 'sensor',
    templateUrl:'./sensor.component.html'
})
export class SensorComponent implements OnInit {
    private _hubConnection = new HubConnectionBuilder().withUrl('/hubs/sensor').build();
    dataStream: ISensorData[] = [];
    subject: BehaviorSubject<ISensorData> = new BehaviorSubject<ISensorData>({timeStamp: new Date(), sensorType: "", sensorValue: 0});
  subject$ = this.subject.asObservable();

    ngOnInit() {
        this.subject.pipe(
            filter(val => val.sensorType === '1'))
            .subscribe({
                next: (value: ISensorData) => this.dataStream.push(value),
                error: function (err) { console.log(err); },
                complete: () => console.log("done")
            });

        this._hubConnection.start()
          .then(() => {
                var obs = this._hubConnection.stream<ISensorData[]>("Values").subscribe({
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
