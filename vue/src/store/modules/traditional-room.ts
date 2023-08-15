import { Store, Module, ActionContext } from 'vuex'
import ListModule from './list-module'
import ListState from './list-state'
import User from '../entities/user'
import Ajax from '../../lib/ajax'
import PageResult from '@/store/entities/page-result';
import ListMutations from './list-mutations'
import UnlockTimeSheet from '../entities/unlock-timesheet'
import TraditionalRoom from '../entities/traditionalRoom'

interface TraditionalRoomState extends ListState<TraditionalRoom> {
}
class NewsMutations extends ListMutations<User>{
}
class TraditionalRoomModule extends ListModule<TraditionalRoomState, any, TraditionalRoom>{
    actions = {
        async getAllAlbum(context:any, payload: any){
            let response = await Ajax.get('/api/services/app/TraditionAlbum/GetAllAlbum')
            return response.data.result
        },
        async create(context:ActionContext<TraditionalRoomState,any>,payload:any){
            await Ajax.post('/api/services/app/TraditionAlbum/Create', payload.data);
        },
        async update(context:ActionContext<TraditionalRoomState,any>,payload:any){
            await Ajax.post('/api/services/app/TraditionAlbum/Update', payload.data);
        },
        async delete(context:ActionContext<TraditionalRoomState,any>,payload:any){
            await Ajax.delete('/api/services/app/TraditionAlbum/Delete?Id='+ payload.data.id);
        }
    };
}
const traditionalRoomModule = new TraditionalRoomModule();
export default traditionalRoomModule;