using Business.CommonBusiness;
using Model.DBTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Common
{
    public class SpinProvider
    {

        private static string GetKey(string key)
        {
            var arr = key.Split(' ');
            return string.Join("_", arr);
        }
        private static string GenDropDownlist(List<SPIN_WORDS> lstSpin, string key)
        {
            key = key.Replace("[[", "").Replace("]]", "");
            string dropdown = "<select name='" + key + "' class='txt-select2' style='min-width:100px;' >";
            foreach (var item in lstSpin)
            {
                dropdown += "  <option value='" + item.TU_CUMTU + "'>" + item.TU_CUMTU + "</option>";
            }


            dropdown += "</select>";
            return dropdown;
        }
        public static SpinMarkBO GenMark(List<GroupTuDienBO> lstTuDien, string content)
        {
            var model = new SpinMarkBO();
            model.Marks = new Dictionary<string, List<SPIN_WORDS>>();
            var result = content;
            foreach (var group in lstTuDien)
            {
                foreach (var item in group.LstWords)
                {
                    if (content.Contains(item.TU_CUMTU))
                    {
                        var Key = "[[" + GetKey(item.TU_CUMTU) + "]]";
                        result = result.Replace(item.TU_CUMTU, Key);
                        model.Marks.Add(Key, group.LstWords);
                    }
                }
            }
            model.Content = result;
            return model;
        }

        public static string GenContentEdit(List<GroupTuDienBO> lstTuDien, string content)
        {
            var spinmark = GenMark(lstTuDien, content);
            var result = spinmark.Content;
            foreach (var item in spinmark.Marks)
            {
                var ddlist = GenDropDownlist(item.Value, item.Key);
                result = result.Replace(item.Key, ddlist);
            }
            return result;
        }

        public static string GetResultContent(List<ParamReplaceSpinBO> param, string content)
        {

            var result = content;
            foreach (var item in param)
            {
                var key = "[[" + item.key + "]]";
                result = result.Replace(key, item.value);
            }
            return result;
        }
    }
}