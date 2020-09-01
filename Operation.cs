using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator
{
    public class Operation
    {

        #region public properties

        public string Left_side { get; set; }
        public string Right_side { get; set; }
        public Operation_type OperationType { get; set; }
        public Operation InnerOperation { get; set; }


        #endregion

        public Operation()
        {
            this.Left_side = string.Empty;
            this.Right_side = string.Empty;
        }

    }
}
