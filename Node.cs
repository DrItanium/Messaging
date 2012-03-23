//Copyright 2012 Joshua Scoggins. All rights reserved.
//
//Redistribution and use in source and binary forms, with or without modification, are
//permitted provided that the following conditions are met:
//
//   1. Redistributions of source code must retain the above copyright notice, this list of
//      conditions and the following disclaimer.
//
//   2. Redistributions in binary form must reproduce the above copyright notice, this list
//      of conditions and the following disclaimer in the documentation and/or other materials
//      provided with the distribution.
//
//THIS SOFTWARE IS PROVIDED BY Joshua Scoggins ``AS IS'' AND ANY EXPRESS OR IMPLIED
//WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
//FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL Joshua Scoggins OR
//CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
//CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
//SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
//ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
//NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
//ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//
//The views and conclusions contained in the software and documentation are those of the
//authors and should not be interpreted as representing official policies, either expressed
//or implied, of Joshua Scoggins. 
using System;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Libraries.Messaging
{
  ///<summary>
  ///Represents a container over a set of actions to perform
  ///with respect to messages. This dispatches actions based 
  ///on messages received. Recieving a ton of messages will
  ///cause it to queue them. Processing them one after another.
  ///</summary>
  public abstract class Node : TaggedObject
  {
    protected Node(Guid id) : base(id) { }
    public virtual void Receive(Message incoming)
    {
      switch(incoming.OperationType)
      {
        case MessageOperationType.Pass:
          OnPass(incoming);
          break;
        case MessageOperationType.Execute:
          OnExecute(incoming);
          break;
        case MessageOperationType.Return:
          OnReturn(incoming);
          break;
        case MessageOperationType.System:
          OnSystemCall(incoming);
          break;
        case MessageOperationType.Init:
          OnInit(incoming);
          break;
        case MessageOperationType.Failure:
          throw new ArgumentException(string.Format("Error: {0}", incoming.Value));
          break;
        default:
          DispatchTo(incoming.Sender, ObjectID, MessageOperationType.Failure, "Invalid Action Given");
          break;
      }
    }
    public virtual void OnPass(Message incoming)
    {
      //do nothing
    }
    public virtual void OnExecute(Message incoming)
    {
      //do nothing
    }
    public virtual void OnReturn(Message incoming)
    {
      //do nothing
    }
    public virtual void OnSystemCall(Message incoming)
    {
      //do nothing
    }
    public virtual void OnInit(Message incoming)
    {
      //do nothing
    }
    public virtual void DispatchTo(Guid target, Guid from, MessageOperationType type, object input)
    {
      DispatchTo(new Message(Guid.NewGuid(), from, target, type, input)); 
    }
    public abstract void DispatchTo(Message mess);
  }
}
