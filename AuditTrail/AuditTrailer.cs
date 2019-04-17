using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AuditTrail
{
    public class AuditTrailer
    {
        /*
         * --<ROLEPERMISSIONS>
            --	<ROLE ID="">
            --		<PERMISSION COMPTYPEID="" SEQNO="" PMASK="" />
            --	</ROLE>
            --</ROLEPERMISSIONS>
         */
        [Microsoft.SqlServer.Server.SqlFunction]
        public static string Create(int roleId, string[] componentTypeIds, string[] sequenceIds, string[] permissionMasks)
        {
            XmlDocument xDoc = new XmlDocument();
            XmlNode root = xDoc.CreateElement("ROLEPERMISSIONS");
            XmlNode roleItem = xDoc.CreateElement("ROLE");
            XmlAttribute role_Id = xDoc.CreateAttribute("ID");
            XmlNode permission = xDoc.CreateElement("PERMISSION");

            role_Id.AppendChild(xDoc.CreateTextNode(roleId.ToString()));

            for (int idx = 0; idx < componentTypeIds.Length - 1; idx++)
            {
                XmlAttribute compType = xDoc.CreateAttribute("COMPTYPEID");
                compType.AppendChild(xDoc.CreateTextNode(componentTypeIds[idx]));

                XmlAttribute sequenceId = xDoc.CreateAttribute("SEQNO");
                sequenceId.AppendChild(xDoc.CreateTextNode(sequenceIds[idx]));

                XmlAttribute permissionMask = xDoc.CreateAttribute("PMASK");
                permissionMask.AppendChild(xDoc.CreateTextNode(permissionMasks[idx]));

                permission.AppendChild(permissionMask);
                permission.AppendChild(sequenceId);
                permission.AppendChild(compType);
            }

            roleItem.AppendChild(permission);
            roleItem.AppendChild(role_Id);
            root.AppendChild(roleItem);

            return root.OuterXml;
        }
    }
}
