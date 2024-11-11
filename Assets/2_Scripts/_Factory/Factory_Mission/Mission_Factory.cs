using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission_Factory : MonoBehaviour
{
    public void Create_Mission(STO_Mission p_Mission, Vector3 p_Position)
    {
        StartCoroutine(C_Create_Mission(p_Mission, p_Position));
    }

    public IEnumerator C_Create_Mission(STO_Mission p_Mission, Vector3 p_Position)
    {
        switch (p_Mission.missionType)
        {
            case MissionType.SURVIVE:
                {
                    GameObject Mission_Go = new GameObject(p_Mission.missionType.ToString());

                    Mission_Go.AddComponent<SurviversMission>();

                    Mission_Go.transform.position = p_Position;
                    Mission_Go.transform.rotation = Quaternion.identity;
                    Mission_Go.transform.localScale = Vector3.one;

                    if (Mission_Go.GetComponent<SurviversMission>())
                    {
                        yield return new WaitUntil(() => Mission_Go.GetComponent<SurviversMission>().SpriteRenderer);
                        Mission_Go.GetComponent<SurviversMission>().MissionSetting(Mission_Go.GetComponent<SurviversMission>(), p_Mission);

                        Mission_Go.GetComponent<Mission>().SpriteRenderer.drawMode = SpriteDrawMode.Sliced;
                        Mission_Go.GetComponent<Mission>().SpriteRenderer.size = new Vector2(2, 2);

                        Mission_Go.GetComponent<SurviversMission>().MissionType = p_Mission.missionType;

                        All_Manager.instance.GameMode_M.missionList.Add(Mission_Go.GetComponent<SurviversMission>());
                        All_Manager.instance.Resource_M.SYGizmo.GizmoTargets.Add(Mission_Go.GetComponent<SurviversMission>().gameObject, 1.0f);
                    }
                }
                break;
            case MissionType.DEFEND:
                {
                    GameObject Mission_Go = new GameObject(p_Mission.missionType.ToString());

                    Mission_Go.AddComponent<DefendMission>();

                    Mission_Go.transform.position = p_Position;
                    Mission_Go.transform.rotation = Quaternion.identity;
                    Mission_Go.transform.localScale = Vector3.one;

                    if (Mission_Go.GetComponent<DefendMission>())
                    {
                        yield return new WaitUntil(() => Mission_Go.GetComponent<DefendMission>().SpriteRenderer);
                        Mission_Go.GetComponent<DefendMission>().MissionSetting(Mission_Go.GetComponent<DefendMission>(), p_Mission);

                        Mission_Go.GetComponent<Mission>().SpriteRenderer.drawMode = SpriteDrawMode.Sliced;
                        Mission_Go.GetComponent<Mission>().SpriteRenderer.size = new Vector2(2, 2);

                        Mission_Go.GetComponent<DefendMission>().MissionType = p_Mission.missionType;

                        All_Manager.instance.GameMode_M.missionList.Add(Mission_Go.GetComponent<DefendMission>());
                        All_Manager.instance.Resource_M.SYGizmo.GizmoTargets.Add(Mission_Go.GetComponent<DefendMission>().gameObject, 1.0f);
                    }
                }
                break;
            case MissionType.FOLLOW:
                {
                    GameObject Mission_Go = new GameObject(p_Mission.missionType.ToString());

                    Mission_Go.AddComponent<FollowMission>();

                    Mission_Go.transform.position = p_Position;
                    Mission_Go.transform.rotation = Quaternion.identity;
                    Mission_Go.transform.localScale = Vector3.one;

                    if (Mission_Go.GetComponent<FollowMission>())
                    {
                        yield return new WaitUntil(() => Mission_Go.GetComponent<FollowMission>().SpriteRenderer);
                        Mission_Go.GetComponent<FollowMission>().MissionSetting(Mission_Go.GetComponent<FollowMission>(), p_Mission);

                        Mission_Go.GetComponent<Mission>().SpriteRenderer.drawMode = SpriteDrawMode.Sliced;
                        Mission_Go.GetComponent<Mission>().SpriteRenderer.size = new Vector2(2, 2);

                        Mission_Go.GetComponent<FollowMission>().MissionType = p_Mission.missionType;

                        All_Manager.instance.GameMode_M.missionList.Add(Mission_Go.GetComponent<FollowMission>());
                        All_Manager.instance.Resource_M.SYGizmo.GizmoTargets.Add(Mission_Go.GetComponent<FollowMission>().gameObject, 1.0f);
                    }
                }
                break;
            default:
                break;
        }

    }


}
