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
    
    ngOnInit() {
        this._hubConnection = new HubConnection('/hubs/sensor');
        this._hubConnection.start()
            .then(() => {
                //var obs = <Observable<SensorData>>this._hubConnection.stream("Values");
                //obs.filter(val => val.SensorValue < 7)
                this._hubConnection.stream("Values")
                .subscribe({
                        next: (value: SensorData) => this.dataStream.push(value),
                        error: function (err) { console.log(err); },
                        complete: () => console.log("done")
                    });
            });
    }
}

class SensorData {
    TimeStamp: Date;
    SensorType: string;
    SensorValue: number;
}
