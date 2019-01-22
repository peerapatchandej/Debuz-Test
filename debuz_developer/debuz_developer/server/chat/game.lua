local Packet = require("chat.packet")

local Game = {}

--------------------------------------------
--- private
--------------------------------------------

local ccu = 0
local users = {}

local function add_user(remote)
  for uid, r in pairs(users) do
    if r.uid == remote.uid then       --ถ้า uid ใหม่เท่ากับ uid ใน users จะให้ uid ใหม่เพิ่ม user เก่าก่อนหน้าทั้งหมด รวมถึงตัวมันเองด้วย
      for uid, n in pairs(users) do
        r:send(Packet.make_add_user(n.uid, n.user, remote.uid))
      end
    else
      for uid, n in pairs(users) do
        r:send(Packet.make_add_user(n.uid, n.user, -1))     --อัปเดตข้อมูลของ user เก่าทั้งหมด รวมถึงตัวมันเองด้วย
      end

      --[[r:send(Packet.make_add_user(remote.uid, remote.user, -1))   --ให้ user เก่าทุกคนเพิ่ม user ใหม่]]--
    end
  end
end

local function remove_user(uid)
  for user, r in pairs(users) do
    r:send(Packet.make_remove_user(uid))
  end
end

--------------------------------------------
--- public
--------------------------------------------

function Game.register_user(remote)
  local uid = #users + 1

  remote.uid = uid
  remote.user = remote.user

  --ตรวจสอบชื่อที่ซ้ำกัน เพื่อเพิ่ม [uid] ท้ายชื่อของ user
  if ccu >= 1 then
    for user, r in pairs(users) do
      local oldUser = string.lower(r.user)
      local newUser = string.lower(remote.user)
      
      if string.find(oldUser, newUser, 1) ~= nil then
        local uidEx = "%[" .. r.uid .. "%]"

        if string.find(r.user, uidEx, 1) == nil and oldUser == newUser then
          r.user = r.user .. "[" .. r.uid .. "]"
          remote.user = remote.user .. "[" .. remote.uid .. "]"
          break
        elseif string.find(r.user, uidEx, 1) ~= nil then
          remote.user = remote.user .. "[" .. remote.uid .. "]"
          break
        end
      end
    end
  end

  users[uid] = remote
  add_user(remote)
  Game.chat(remote.user, "joined", remote.uid, "group")

  ccu = ccu + 1
  print("CCU = " .. ccu)
end

function Game.unregister_user(remote)
  local uid = remote.uid
  local user = remote.user

  users[uid] = nil

  remote.uid = nil
  remote.user = nil

  remove_user(uid)

  Game.chat(user, "leave room", remote.uid, "group")

  ccu = ccu - 1
  print("CCU = " .. ccu)
end

function Game.chat(user, message, playerSrc, playerDes)
  for uid, r in pairs(users) do
    if playerDes == "group" then
      r:send(Packet.make_chat(user, message, playerDes))
    elseif r.uid == tonumber(playerDes) then
      r:send(Packet.make_chat(user, message, playerSrc))
    elseif playerDes ~= "group" and r.user == user then
      r:send(Packet.make_chat(user, message, playerDes))
    end
  end
end

return Game
