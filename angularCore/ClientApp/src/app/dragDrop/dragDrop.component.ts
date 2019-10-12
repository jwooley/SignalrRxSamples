import { Component, OnInit } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { CdkDragMove } from '@angular/cdk/drag-drop';
@Component({
    selector: 'app-dragdrop',
    templateUrl: './dragDrop.component.html'
})
export class DragDropComponent implements OnInit {
    _hubConnection: HubConnection;
    state = '';
    pointerPositionX: number;
    pointerPositionY: number;

    ngOnInit() {
        this._hubConnection = new HubConnectionBuilder().withUrl('/hubs/dragdrop').build();

        this._hubConnection.start()
          .then(() => {
            console.log('Hub connection started');

              this._hubConnection.on('Dragged', (position: any) => {
                  this.state = 'External';
                  this.pointerPositionX = position.x;
                  this.pointerPositionY = position.y;
            });
          })
          .catch(err => { console.log('Error establishing connection'); }
          );
    }

    dragMoved(event: CdkDragMove) {
        this._hubConnection.invoke('DragDrop', { x: event.pointerPosition.x, y: event.pointerPosition.y });
        this.pointerPositionX = event.pointerPosition.x;
        this.pointerPositionY = event.pointerPosition.y;
    }
}
