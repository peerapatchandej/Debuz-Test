local Class      = require("omg.class")
local Game       = require("chat.game")

local Remote     = Class("chat.remote")

------------------------------------------------------------------------------
-- public
------------------------------------------------------------------------------

function Remote:__init(socket)
  self.socket = socket
  self.user = nil
  self.uid = nil
end

function Remote:disconnected()
  if self.uid then
    Game.unregister_user(self)
  end
  self.socket = nil
end

function Remote:send(packet)
  self.socket:send(packet)
end

function Remote:close()
  self.socket:shutdown()
end

function Remote:recv_login(user)
  --[[Game.chat(user, "joined")]]--

  self.user = user
  Game.register_user(self)
end

function Remote:recv_chat(message, playerSrc, playerDes)
  Game.chat(self.user, message, playerSrc, playerDes)
end

return Remote
