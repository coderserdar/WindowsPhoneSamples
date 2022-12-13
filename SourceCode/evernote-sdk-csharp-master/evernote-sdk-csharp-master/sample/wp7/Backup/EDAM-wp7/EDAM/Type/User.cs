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
  public partial class User : TBase
  {
    private int _id;
    private string _username;
    private string _email;
    private string _name;
    private string _timezone;
    private PrivilegeLevel _privilege;
    private long _created;
    private long _updated;
    private long _deleted;
    private bool _active;
    private string _shardId;
    private UserAttributes _attributes;
    private Accounting _accounting;
    private PremiumInfo _premiumInfo;
    private BusinessUserInfo _businessUserInfo;

    public int Id
    {
      get
      {
        return _id;
      }
      set
      {
        __isset.id = true;
        this._id = value;
      }
    }

    public string Username
    {
      get
      {
        return _username;
      }
      set
      {
        __isset.username = true;
        this._username = value;
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

    public string Name
    {
      get
      {
        return _name;
      }
      set
      {
        __isset.name = true;
        this._name = value;
      }
    }

    public string Timezone
    {
      get
      {
        return _timezone;
      }
      set
      {
        __isset.timezone = true;
        this._timezone = value;
      }
    }

    public PrivilegeLevel Privilege
    {
      get
      {
        return _privilege;
      }
      set
      {
        __isset.privilege = true;
        this._privilege = value;
      }
    }

    public long Created
    {
      get
      {
        return _created;
      }
      set
      {
        __isset.created = true;
        this._created = value;
      }
    }

    public long Updated
    {
      get
      {
        return _updated;
      }
      set
      {
        __isset.updated = true;
        this._updated = value;
      }
    }

    public long Deleted
    {
      get
      {
        return _deleted;
      }
      set
      {
        __isset.deleted = true;
        this._deleted = value;
      }
    }

    public bool Active
    {
      get
      {
        return _active;
      }
      set
      {
        __isset.active = true;
        this._active = value;
      }
    }

    public string ShardId
    {
      get
      {
        return _shardId;
      }
      set
      {
        __isset.shardId = true;
        this._shardId = value;
      }
    }

    public UserAttributes Attributes
    {
      get
      {
        return _attributes;
      }
      set
      {
        __isset.attributes = true;
        this._attributes = value;
      }
    }

    public Accounting Accounting
    {
      get
      {
        return _accounting;
      }
      set
      {
        __isset.accounting = true;
        this._accounting = value;
      }
    }

    public PremiumInfo PremiumInfo
    {
      get
      {
        return _premiumInfo;
      }
      set
      {
        __isset.premiumInfo = true;
        this._premiumInfo = value;
      }
    }

    public BusinessUserInfo BusinessUserInfo
    {
      get
      {
        return _businessUserInfo;
      }
      set
      {
        __isset.businessUserInfo = true;
        this._businessUserInfo = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT && !NETFX_CORE
    [Serializable]
    #endif
    public struct Isset {
      public bool id;
      public bool username;
      public bool email;
      public bool name;
      public bool timezone;
      public bool privilege;
      public bool created;
      public bool updated;
      public bool deleted;
      public bool active;
      public bool shardId;
      public bool attributes;
      public bool accounting;
      public bool premiumInfo;
      public bool businessUserInfo;
    }

    public User() {
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
              Id = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.String) {
              Username = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.String) {
              Email = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.String) {
              Name = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 6:
            if (field.Type == TType.String) {
              Timezone = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 7:
            if (field.Type == TType.I32) {
              Privilege = (PrivilegeLevel)iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 9:
            if (field.Type == TType.I64) {
              Created = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 10:
            if (field.Type == TType.I64) {
              Updated = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 11:
            if (field.Type == TType.I64) {
              Deleted = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 13:
            if (field.Type == TType.Bool) {
              Active = iprot.ReadBool();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 14:
            if (field.Type == TType.String) {
              ShardId = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 15:
            if (field.Type == TType.Struct) {
              Attributes = new UserAttributes();
              Attributes.Read(iprot);
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 16:
            if (field.Type == TType.Struct) {
              Accounting = new Accounting();
              Accounting.Read(iprot);
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 17:
            if (field.Type == TType.Struct) {
              PremiumInfo = new PremiumInfo();
              PremiumInfo.Read(iprot);
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 18:
            if (field.Type == TType.Struct) {
              BusinessUserInfo = new BusinessUserInfo();
              BusinessUserInfo.Read(iprot);
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
      TStruct struc = new TStruct("User");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.id) {
        field.Name = "id";
        field.Type = TType.I32;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(Id);
        oprot.WriteFieldEnd();
      }
      if (Username != null && __isset.username) {
        field.Name = "username";
        field.Type = TType.String;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Username);
        oprot.WriteFieldEnd();
      }
      if (Email != null && __isset.email) {
        field.Name = "email";
        field.Type = TType.String;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Email);
        oprot.WriteFieldEnd();
      }
      if (Name != null && __isset.name) {
        field.Name = "name";
        field.Type = TType.String;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Name);
        oprot.WriteFieldEnd();
      }
      if (Timezone != null && __isset.timezone) {
        field.Name = "timezone";
        field.Type = TType.String;
        field.ID = 6;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Timezone);
        oprot.WriteFieldEnd();
      }
      if (__isset.privilege) {
        field.Name = "privilege";
        field.Type = TType.I32;
        field.ID = 7;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)Privilege);
        oprot.WriteFieldEnd();
      }
      if (__isset.created) {
        field.Name = "created";
        field.Type = TType.I64;
        field.ID = 9;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(Created);
        oprot.WriteFieldEnd();
      }
      if (__isset.updated) {
        field.Name = "updated";
        field.Type = TType.I64;
        field.ID = 10;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(Updated);
        oprot.WriteFieldEnd();
      }
      if (__isset.deleted) {
        field.Name = "deleted";
        field.Type = TType.I64;
        field.ID = 11;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(Deleted);
        oprot.WriteFieldEnd();
      }
      if (__isset.active) {
        field.Name = "active";
        field.Type = TType.Bool;
        field.ID = 13;
        oprot.WriteFieldBegin(field);
        oprot.WriteBool(Active);
        oprot.WriteFieldEnd();
      }
      if (ShardId != null && __isset.shardId) {
        field.Name = "shardId";
        field.Type = TType.String;
        field.ID = 14;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(ShardId);
        oprot.WriteFieldEnd();
      }
      if (Attributes != null && __isset.attributes) {
        field.Name = "attributes";
        field.Type = TType.Struct;
        field.ID = 15;
        oprot.WriteFieldBegin(field);
        Attributes.Write(oprot);
        oprot.WriteFieldEnd();
      }
      if (Accounting != null && __isset.accounting) {
        field.Name = "accounting";
        field.Type = TType.Struct;
        field.ID = 16;
        oprot.WriteFieldBegin(field);
        Accounting.Write(oprot);
        oprot.WriteFieldEnd();
      }
      if (PremiumInfo != null && __isset.premiumInfo) {
        field.Name = "premiumInfo";
        field.Type = TType.Struct;
        field.ID = 17;
        oprot.WriteFieldBegin(field);
        PremiumInfo.Write(oprot);
        oprot.WriteFieldEnd();
      }
      if (BusinessUserInfo != null && __isset.businessUserInfo) {
        field.Name = "businessUserInfo";
        field.Type = TType.Struct;
        field.ID = 18;
        oprot.WriteFieldBegin(field);
        BusinessUserInfo.Write(oprot);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("User(");
      sb.Append("Id: ");
      sb.Append(Id);
      sb.Append(",Username: ");
      sb.Append(Username);
      sb.Append(",Email: ");
      sb.Append(Email);
      sb.Append(",Name: ");
      sb.Append(Name);
      sb.Append(",Timezone: ");
      sb.Append(Timezone);
      sb.Append(",Privilege: ");
      sb.Append(Privilege);
      sb.Append(",Created: ");
      sb.Append(Created);
      sb.Append(",Updated: ");
      sb.Append(Updated);
      sb.Append(",Deleted: ");
      sb.Append(Deleted);
      sb.Append(",Active: ");
      sb.Append(Active);
      sb.Append(",ShardId: ");
      sb.Append(ShardId);
      sb.Append(",Attributes: ");
      sb.Append(Attributes== null ? "<null>" : Attributes.ToString());
      sb.Append(",Accounting: ");
      sb.Append(Accounting== null ? "<null>" : Accounting.ToString());
      sb.Append(",PremiumInfo: ");
      sb.Append(PremiumInfo== null ? "<null>" : PremiumInfo.ToString());
      sb.Append(",BusinessUserInfo: ");
      sb.Append(BusinessUserInfo== null ? "<null>" : BusinessUserInfo.ToString());
      sb.Append(")");
      return sb.ToString();
    }

  }

}
