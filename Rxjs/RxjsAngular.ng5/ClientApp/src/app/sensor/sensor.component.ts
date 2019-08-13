import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { filter } from 'rxjs/operators';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';

@Component({
    selector: 'app-sensor',
    templateUrl: './sensor.component.html'
})
export class SensorComponent implements OnInit {
    private _hubConnection = new HubConnectionBuilder().withUrl('/hubs/sensor').build();
    dataStream: ISensorData[] = [];
    subject: BehaviorSubject<ISensorData> = new BehaviorSubject<ISensorData>({timeStamp: new Date(), sensorType: '', sensorValue: 0});

    ngOnInit() {
      this.subject
          .pipe(
              filter(val => val.sensorValue > 15)
            ).subscribe({
                next: (value: ISensorData) => this.dataStream.push(value),
                error: function (err) { console.log(err); },
                complete: () => console.log('done')
            });

        this._hubConnection.start()
            .then(() => {
                const obs = this._hubConnection.stream<ISensorData>('Values').subscribe({
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
