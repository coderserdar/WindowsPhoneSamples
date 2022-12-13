/**
 * Autogenerated by Thrift
 *
 * DO NOT EDIT UNLESS YOU ARE SURE THAT YOU KNOW WHAT YOU ARE DOING
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Thrift;
using Thrift.Collections;
using Thrift.Protocol;
using Thrift.Transport;
namespace Evernote.EDAM.Type
{

  #if !SILVERLIGHT && !NETFX_CORE
  [Serializable]
  #endif
  public partial class BusinessUserInfo : TBase
  {
    private int _businessId;
    private string _businessName;
    private BusinessUserRole _role;
    private string _email;

    public int BusinessId
    {
      get
      {
        return _businessId;
      }
      set
      {
        __isset.businessId = true;
        this._businessId = value;
      }
    }

    public string BusinessName
    {
      get
      {
        return _businessName;
      }
      set
      {
        __isset.businessName = true;
        this._businessName = value;
      }
    }

    public BusinessUserRole Role
    {
      get
      {
        return _role;
      }
      set
      {
        __isset.role = true;
        this._role = value;
      }
    }

    public string Email
    {
      get
      {
        return _email;
      }
      set
      {
        __isset.email = true;
        this._email = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT && !NETFX_CORE
    [Serializable]
    #endif
    public struct Isset {
      public bool businessId;
      public bool businessName;
      public bool role;
      public bool email;
    }

    public BusinessUserInfo() {
    }

    public void Read (TProtocol iprot)
    {
      TField field;
      iprot.ReadStructBegin();
      while (true)
      {
        field = iprot.ReadFieldBegin();
        if (field.Type == TType.Stop) { 
          break;
        }
        switch (field.ID)
        {
          case 1:
            if (field.Type == TType.I32) {
              BusinessId = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.String) {
              BusinessName = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.I32) {
              Role = (BusinessUserRole)iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.String) {
              Email = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          default: 
            TProtocolUtil.Skip(iprot, field.Type);
            break;
        }
        iprot.ReadFieldEnd();
      }
      iprot.ReadStructEnd();
    }

    public void Write(TProtocol oprot) {
      TStruct struc = new TStruct("BusinessUserInfo");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.businessId) {
        field.Name = "businessId";
        field.Type = TType.I32;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(BusinessId);
        oprot.WriteFieldEnd();
      }
      if (BusinessName != null && __isset.businessName) {
        field.Name = "businessName";
        field.Type = TType.String;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(BusinessName);
        oprot.WriteFieldEnd();
      }
      if (__isset.role) {
        field.Name = "role";
        field.Type = TType.I32;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)Role);
        oprot.WriteFieldEnd();
      }
      if (Email != null && __isset.email) {
        field.Name = "email";
        field.Type = TType.String;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Email);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("BusinessUserInfo(");
      sb.Append("BusinessId: ");
      sb.Append(BusinessId);
      sb.Append(",BusinessName: ");
      sb.Append(BusinessName);
      sb.Append(",Role: ");
      sb.Append(Role);
      sb.Append(",Email: ");
      sb.Append(Email);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
