namespace ContactViewer
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Controls;
    
    //using ContactService;
    
    public class ViewModel : INotifyPropertyChanged
    {
        public ViewContact CurrentContact { get; set; }

        public List<ViewContact> ResultList { get; set; }

        public void SearchForContact()
        {
            //var service = new ContactViewServiceClient();
            //service.GetAllCompleted += ServiceGetAllCompleted;
            //service.GetAllAsync("");
        }

        //private void ServiceGetAllCompleted(object sender, GetAllCompletedEventArgs e)
        //{
        //    this.ResultList =
        //        (List<ViewContact>)(from x in e.Result
        //                            select new ViewContact
        //                                       {
        //                                           FullName = x.FullName
        //                                       });

        //    if (PropertyChanged != null)
        //        PropertyChanged(this, new PropertyChangedEventArgs("ResultList"));
        //}

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }

    public class ViewContact
    {
        public string FullName { get; set; }
        public Image Picture { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
    }
}
