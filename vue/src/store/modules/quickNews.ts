import Ajax from '../../lib/ajax';
import ListModule from './list-module';
import ListState from './list-state';
interface QuickNewsState extends ListState<any>{
    // editTenant:TinyMce;
}
class  QuickNewsModule {

    actions = {
        // async getAllQuickNews() {
        //     let reponse = await Ajax.get('/api/services/app/QuickNew/GetAllNews');
        //     return reponse.data.result
        // },
        async getAllPolicies( payload:any){
            let reponse = await Ajax.post('/api/services/app/QuickNewsAppServices/GetAllPaging',payload.data)
            return reponse.data.result
        },
    };

}
const quickNewsModule = new QuickNewsModule();
export default quickNewsModule;